using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class autoResizeLabel : MonoBehaviour
{
    public RectTransform rectToSize;
    public TextMeshProUGUI label;
    public BoxCollider bCollider;


    public enum labelAlignment { RIGHT,LEFT,TOP,BOTTOM}


    public float charWidth = 2.32f;
    public float charHeight = 3f;
    [Tooltip("the rects x,y,width,height is right,left,up,down padding respectively")]
    public Rect padding = new Rect(11.6f, 11.6f, 3.5f, 3.5f);
    [SerializeField]
    private labelAlignment labelAlign;

    public string text = "some thing is \n not right!";


    void Start()
    {
        resizeLabel();
    }


    public void resizeLabel()
    {
        // first replace \n with | so we can split the string
        text = text.Replace("\\n", "|");
        // splitting the string
        string[] line = text.Split('|');
        // after that we revert back to the \n
        text = text.Replace("|", "\n");
        // we replace back \n with acutal \n
        // so textmesh pro will properly make a new line
        text = text.Replace("\\n", "\n");
        
        int longestLine = 0;

        
        // we get the char count of the longest line
        // to use it in determining the width of the label
        for (int i = 0; i < line.Length; i++)
        {
            if(longestLine == 0)
            {
                longestLine = line[i].Length;
            }
            else
            {
                if(line[i].Length > longestLine)
                {
                    longestLine = line[i].Length;
                }
            }
        }

        Debug.Log(longestLine.ToString());
        // width is calculated using the longestline and
        // right and left padding
        float width = (longestLine * charWidth) + padding.x+ padding.y;
        // height is determined by the number of new lines and
        // upper and lower padding
        float height =(line.Length * charHeight) + padding.width+ padding.height;
        rectToSize.sizeDelta = new Vector2(width, height);
        bCollider.size = new Vector3(width, height, 1f);
        bCollider.center = Vector3.zero;
        label.text = text;
        alignLabel(width, height);
    }


    void alignLabel(float width, float height)
    {
        switch (labelAlign)
        {
            case labelAlignment.RIGHT:
                rectToSize.anchoredPosition = new Vector3(width / 2, 0, 0);
                break;
            case labelAlignment.LEFT:
                rectToSize.anchoredPosition = new Vector3((width / 2)*-1, 0, 0);
                break;
            case labelAlignment.TOP:
                rectToSize.anchoredPosition = new Vector3(0, (height / 2) * -1, 0);
                break;
            case labelAlignment.BOTTOM:
                rectToSize.anchoredPosition = new Vector3(0, height / 2, 0);
                break;
            default:
                break;
        }
    }
}


