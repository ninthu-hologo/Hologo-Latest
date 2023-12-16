using System;
using System.Collections.Generic;
using DiffMatchPatch;
using UnityEngine;
using UnityEngine.Assertions;

namespace SoftMasking.TextMeshPro.Editor {
    [Serializable] public enum TextMeshProType { Any, Binary, Source }
    [Serializable] public enum TextMeshProVersion { Any }

    [CreateAssetMenu]
    public class ShaderPatchSet : ScriptableObject {
        public enum MatchLevel { Full = 2, Partial = 1, DontMatch = 0 }

        [Serializable]
        public class Entry {
            public string shaderName;
            public List<Patch> patches;
            public TextMeshProType textMeshProType;
            public TextMeshProVersion textMeshProVersion;

            public Entry(
                    string shaderName,
                    List<Patch> patches,
                    TextMeshProType tmproType,
                    TextMeshProVersion tmproVersion) {
                this.shaderName = shaderName;
                this.patches = patches;
                this.textMeshProType = tmproType;
                this.textMeshProVersion = tmproVersion;
            }

            public MatchLevel Match(TextMeshProType requiredType, TextMeshProVersion requiredVersion) {
                return Min(Match(requiredType), Match(requiredVersion));
            }

            MatchLevel Match(TextMeshProType requiredType) {
                Assert.AreNotEqual(TextMeshProType.Any, requiredType);
                if (textMeshProType == requiredType) return MatchLevel.Full;
                if (textMeshProType == TextMeshProType.Any) return MatchLevel.Partial;
                return MatchLevel.DontMatch;
            }

            MatchLevel Match(TextMeshProVersion requiredVersion) { return MatchLevel.Full; }

            static MatchLevel Min(MatchLevel l1, MatchLevel l2) {
                return (MatchLevel)Math.Min((int)l1, (int)l2);
            }
        }

        public List<Entry> entries;

        public Entry Find(
                string shaderName,
                TextMeshProType tmproType,
                TextMeshProVersion tmproVersion) {
            Entry fallback = null;
            foreach (var entry in entries) {
                if (entry.shaderName != shaderName)
                    continue;
                var matchLevel = entry.Match(tmproType, tmproVersion);
                switch (matchLevel) {
                    case MatchLevel.Full: return entry;
                    case MatchLevel.Partial:
                        Assert.IsNull(fallback); // There should be no more than one fallback
                        fallback = entry;
                        break;
                }
            }
            return fallback;
        }

    }
}
