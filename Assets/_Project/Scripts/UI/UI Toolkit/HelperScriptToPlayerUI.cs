using System;
using Unity.Mathematics;
using Unity.Properties;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.UIElements;


[UxmlElement]
public partial class HelperScriptToPlayerUI : VisualElement
{

    [DontCreateProperty]
    private float m_radius;

    [DontCreateProperty]
    private float m_currentFillLevel;

    [DontCreateProperty]
    private float m_width = 10f;

    [DontCreateProperty]
    private Color m_fillColor;

    [UxmlAttribute, CreateProperty]
    public float m_maxFillLevel;

    [UxmlAttribute, CreateProperty]
    public Color _fillColor
    {
        get => m_fillColor;
        set
        {
            if (m_fillColor == value) return;
            m_fillColor = value;
            MarkDirtyRepaint();
        }
    }

    [UxmlAttribute, CreateProperty]
    public float _width
    {
        get => m_width;
        set
        {
            if (m_width == value) return;
            m_width = value;
            MarkDirtyRepaint();
        }
    }

    [UxmlAttribute, CreateProperty]
    public float _radius
    {
        get => m_radius;
        set
        {
            if (m_radius == value) return;
            m_radius = value;
            MarkDirtyRepaint();
        }
    }

    [SerializeField, CreateProperty]
    public float _currentFillLevel
    {
        get => m_currentFillLevel;
        set
        {
            if (m_currentFillLevel == value) return;
            m_currentFillLevel = value;
            MarkDirtyRepaint();
        }
    }


    public HelperScriptToPlayerUI()
    {
        generateVisualContent += GenerateVisualContent;
    }

    public void GenerateVisualContent(MeshGenerationContext context)
    {
        DrawBar(context, m_fillColor, _width);
        // DrawFillLevel(context);
    }

    private void DrawBar(MeshGenerationContext context, Color color, float width)
    {
        var painter = context.painter2D;
        painter.BeginPath();
        painter.lineWidth = width;
        painter.strokeColor = color;



        painter.Arc(new Vector2(0.5f, 0.5f), m_radius, 0f, 360f);
        painter.Stroke();
    }
    private void DrawFillLevel(MeshGenerationContext context)
    {
        var fillLevel = m_currentFillLevel / m_maxFillLevel;
        style.width = Length.Percent(fillLevel * 100);
    }
    
    // private float StartAngle(float segment) => 360f * ;
    // private float EndAngle(float segment) => 360f * ((segment + 1) / SegmentCount);
}
