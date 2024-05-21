using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MINELab
{
    public class GraphicalDisplay : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<GraphicalDisplay, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            // The progress property is exposed to UXML.
            UxmlIntAttributeDescription _minAttributeDescription = new UxmlIntAttributeDescription() { name = "xMin" };
            UxmlIntAttributeDescription _maxAttributeDescription = new UxmlIntAttributeDescription() { name = "xMax" };
            UxmlFloatAttributeDescription _gridLineWidthAttributeDescription = new UxmlFloatAttributeDescription() { name = "gridLineWidth" };
            UxmlFloatAttributeDescription _lineWidthAttributeDescription = new UxmlFloatAttributeDescription() { name = "lineWidth" };

            // Use the Init method to assign the value of the progress UXML attribute to the C# progress property.
            public override void Init(VisualElement visualElement, IUxmlAttributes bag, CreationContext cc)
            {
                if (visualElement is GraphicalDisplay graphicalDisplay)
                {
                    graphicalDisplay.XMin = _minAttributeDescription.GetValueFromBag(bag, cc);
                    graphicalDisplay.XMax = _maxAttributeDescription.GetValueFromBag(bag, cc);
                    graphicalDisplay.GridLineWidth = _gridLineWidthAttributeDescription.GetValueFromBag(bag, cc);
                    graphicalDisplay.LineWidth = _lineWidthAttributeDescription.GetValueFromBag(bag, cc);
                        
                    base.Init(graphicalDisplay, bag, cc);
                }
            }
        }

        private int _xMin;
        public int XMin
        {
            get => _xMin;
            set
            {
                _xMin = value;
                MarkDirtyRepaint();
            }
        }
        
        private int _xMax;
        public int XMax
        {
            get => _xMax;
            set
            {
                _xMax = value;
                MarkDirtyRepaint();
            }
        }
        
        private float _lineWidth;
        public float LineWidth
        {
            get => _lineWidth;
            set
            {
                _lineWidth = value;
                MarkDirtyRepaint();
            }
        }
        
        private float _gridLineWidth;
        public float GridLineWidth
        {
            get => _gridLineWidth;
            set
            {
                _gridLineWidth = value;
                MarkDirtyRepaint();
            }
        }

        private List<float> _data;
        public List<float> Data
        {
            get => _data;
            set
            {
                _data = value;
                MarkDirtyRepaint();
            }
        }
        
        
        
        public GraphicalDisplay()
        {
            generateVisualContent += GenerateVisualContent;
            TimelineController.Instance.timeIndexChangeEvent.AddListener(Refresh);
        }
        
        private void Refresh(int temp) => MarkDirtyRepaint();
        
        void GenerateVisualContent(MeshGenerationContext context)
        {
            Vector2 dimensions = new Vector2(contentRect.width, contentRect.height);
            
            var painter = context.painter2D;
            DrawBackgroundGrid(painter, dimensions);
            DrawCurrentTime(painter, dimensions);
            DrawData(painter, dimensions);
        }

        private void DrawBackgroundGrid(in Painter2D painter, in Vector2 dimensions)
        {
            painter.lineCap = LineCap.Butt;
            painter.lineWidth = LineWidth;

            int totalSpan = _xMax - _xMin;
            int lineIncrement = totalSpan switch
            {
                int lines when (lines < 100) => 1,
                int lines when (lines < 500) => 5,
                int lines when (lines < 1000) => 10,
                int lines when (lines < 5000) => 50,
                _ => 100,
            };

            int startingIndex = (_xMin - (_xMin % lineIncrement));
            for (int i = startingIndex; (i < (totalSpan + _xMin)); i += lineIncrement)
            {
                float xPosition = Mathf.InverseLerp(_xMin, _xMax, i) * dimensions.x;
                
                painter.strokeColor = Color.black;
                painter.BeginPath();
                painter.LineTo(new Vector2(xPosition, dimensions.y));
                painter.LineTo(new Vector2(xPosition, 0));
                painter.Stroke();
            }
        }

        private void DrawData(in Painter2D painter, in Vector2 dimensions)
        {
            if (_data is not { Count: > 0 }) return;
            
            float minYHeight = _data[0];
            float maxYHeight = _data[0];
            for (int i = 1; i < _data.Count; i++)
            {
                if (minYHeight > _data[i]) minYHeight = _data[i];
                if (maxYHeight < _data[i]) maxYHeight = _data[i];
            }
            
            DrawDataLine(painter, dimensions, minYHeight, maxYHeight,GridLineWidth * 1.5f, Color.black);
            DrawDataLine(painter, dimensions, minYHeight, maxYHeight,GridLineWidth, painter.strokeColor = new Color(1f, 0.67f,0.17f));
        }

        private void DrawDataLine(in Painter2D painter, in Vector2 dimensions, in float minY, in float maxY, in float lineWidth, in Color colour)
        {
            painter.lineCap = LineCap.Butt;
            
            painter.strokeColor = colour;
            painter.lineWidth = lineWidth;
            painter.BeginPath();

            Vector2 position;
            painter.LineTo(position = new Vector2()
            {
                x = dimensions.x * Mathf.InverseLerp(_xMin, _xMax, 0),
                y = dimensions.y * (1 - ((Mathf.InverseLerp(minY, maxY, _data[0]) * 0.8f) + 0.1f))
            });

            Vector2 previousPosition = position;
            for (int i = 0; i < _data.Count; i++)
            {
                if (_xMin > i || _xMax < i) continue;
                position = new Vector2()
                {
                    x = dimensions.x * Mathf.InverseLerp(_xMin, _xMax, i),
                    y = dimensions.y * (1 - ((Mathf.InverseLerp(minY, maxY, _data[i]) * 0.8f) + 0.1f))
                };
                
                if ((previousPosition - position).sqrMagnitude < 1) continue;

                painter.LineTo(position);
                previousPosition = position;
            }
            painter.Stroke();
        }

        private void DrawCurrentTime(in Painter2D painter, in Vector2 dimensions)
        {
            if (TimelineController.Instance is not { } timelineController) return;
            float xPosition = Mathf.InverseLerp(_xMin, _xMax, timelineController.TimeIndex) * dimensions.x;
            
            Debug.Log($"Test: {Mathf.InverseLerp(_xMin, _xMax, timelineController.TimeIndex)}");
            
            painter.strokeColor = Color.green;
            painter.BeginPath();
            painter.LineTo(new Vector2(xPosition, dimensions.y));
            painter.LineTo(new Vector2(xPosition, 0));
            painter.Stroke();
        }
    }
}