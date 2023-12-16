using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DiffMatchPatch;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace SoftMasking.TextMeshPro.Editor {
    public static class ShaderPatcher {
        public class ShaderResource {
            public readonly Shader shader;
            public readonly string path;
            public readonly string text;
            public readonly string name;

            public ShaderResource(string path) {
                this.path = path;
                this.shader = AssetDatabase.LoadAssetAtPath<Shader>(path);
                this.text = ReadResource(path);
                this.name = Path.GetFileNameWithoutExtension(path);
            }
        }

        [MenuItem("Tools/Soft Mask/Update TestMesh Pro Integration")]
        public static void UpdateShaders() {
            var diff = new diff_match_patch();
            var patchSet = PackageResources.patchSet;
            Assert.IsNotNull(patchSet);
            var tmproShaders = CollectTMProShaders().ToList();
            if (tmproShaders.Count == 0) {
                Debug.LogError(
                    "Could not update integration because TextMesh Pro shaders are not found. " +
                    "Make sure that TextMesh Pro package is installed.");
                return;
            }
            foreach (var shader in tmproShaders) {
                var entry = patchSet.Find(shader.name, textMeshProType, textMeshProVersion);
                if (entry == null) {
                    Debug.LogErrorFormat("Could not find appropriate patch for shader '{0}'", shader.name);
                    continue;
                }
                var result = diff.patch_apply(entry.patches, shader.text);
                var newText = (string)result[0];
                var applicationResults = (bool[])result[1];
                var succeeded = applicationResults.All(x => x);
                if (succeeded) {
                    var replacementFileName = shader.name + " - SoftMask.shader";
                    if (!Directory.Exists(PackageResources.resourcesPath))
                        Directory.CreateDirectory(PackageResources.resourcesPath);
                    var outputFile = Path.Combine(PackageResources.resourcesPath, replacementFileName);
                    File.WriteAllText(outputFile, UpdateIncludes(newText));
                    AssetDatabase.ImportAsset(outputFile);
                } else {
                    Debug.LogErrorFormat(
                        "Could not patch TextMesh Pro shader {0}. " +
                        "Probably TextMesh Pro has been updated but Soft Mask for TextMesh Pro has not.",
                        shader.name);
                }
            }
            InvalidateSoftMasks();
        }

        static TextMeshProType s_textMeshProType = TextMeshProType.Any;
        public static TextMeshProType textMeshProType {
            get {
                if (s_textMeshProType == TextMeshProType.Any)
                    s_textMeshProType = DetermineTextMeshProType();
                return s_textMeshProType;
            }
        }

        public static TextMeshProVersion textMeshProVersion {
            get { return TextMeshProVersion.Any; }
        }

        public static IEnumerable<ShaderResource> CollectTMProShaders() {
            return
                TMProShaderGUIDs
                    .Select(x => AssetDatabase.GUIDToAssetPath(x))
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(x => new ShaderResource(x))
                    .Where(x => CheckIsUIShader(x.shader));
        }

        static readonly List<string> TMProShaderGUIDs = new List<string> {
            "d1cf17907700cb647aa3ea423ba38f2e", // TMP_Bitmap-Mobile
            "edfcf888cd11d9245b91d2883049a57e", // TMP_Bitmap
            "afc255f7c2be52e41973a3d10a2e632d", // TMP_SDF-Mobile Masking
            "cafd18099dfc0114896e0a8b277b81b6", // TMP_SDF-Mobile
            "dca26082f9cb439469295791d9f76fe5", // TMP_SDF
            "3a1c68c8292caf046bd21158886c5e40"  // TMP_Sprite
        };

        static Dictionary<string, string> s_knownIncludes;
        static Dictionary<string, string> knownIncludes {
            get {
                if (s_knownIncludes == null)
                    s_knownIncludes = CollectIncludes().ToDictionary(x => Path.GetFileName(x));
                return s_knownIncludes;
            }
        }

        static string UpdateInclude(string includePath) {
            string result;
            return
                knownIncludes.TryGetValue(Path.GetFileName(includePath), out result)
                    ? result
                    : includePath;
        }

        static string UpdateIncludes(string shaderCode) {
            return
                Regex.Replace(
                    shaderCode,
                    "#include \"(.+)\"",
                    match => string.Format(
                        "#include \"{0}\"",
                        UpdateInclude(match.Groups[1].Value)));
        }

        static IEnumerable<string> CollectIncludes() {
            return CollectTMProIncludes().Concat(CollectSoftMaskIncludes());
        }

        static IEnumerable<string> CollectTMProIncludes() { return FindIncludes("TMPro"); }
        static IEnumerable<string> CollectSoftMaskIncludes() { return FindIncludes("SoftMask"); }

        static IEnumerable<string> FindIncludes(string filter) {
            return FindAssets(filter).Where(x => x.EndsWith(".cginc"));
        }

        static IEnumerable<string> FindAssets(string filter) {
            return AssetDatabase.FindAssets(filter).Select(x => AssetDatabase.GUIDToAssetPath(x));
        }

        static bool CheckIsUIShader(Shader shader) {
            var material = new Material(shader);
            material.hideFlags = HideFlags.HideAndDontSave;
            var result = material.HasProperty(Ids.Stencil);
            Object.DestroyImmediate(material);
            return result;
        }

        static string ReadResource(string path) { return File.ReadAllText(path); }

        static void InvalidateSoftMasks() {
            var softMaskPath = FindAssets("t:script SoftMask").FirstOrDefault();
            if (!string.IsNullOrEmpty(softMaskPath))
                AssetDatabase.ImportAsset(softMaskPath);
        }

        static readonly string TMProScriptGUID = "1a1578b9753d2604f98d608cb4239e2f"; // TextMeshPro.cs

        static TextMeshProType DetermineTextMeshProType() {
            return !string.IsNullOrEmpty(AssetDatabase.GUIDToAssetPath(TMProScriptGUID))
                ? TextMeshProType.Source
                : TextMeshProType.Binary;
        }

        static class Ids {
            public static readonly int Stencil = Shader.PropertyToID("_Stencil");
        }
    }
}
