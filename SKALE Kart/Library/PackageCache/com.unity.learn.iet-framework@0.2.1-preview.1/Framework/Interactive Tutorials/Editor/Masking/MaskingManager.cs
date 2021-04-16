using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

#if UNITY_2019_1_OR_NEWER
using UnityEngine.UIElements;
using VisualContainer = UnityEngine.UIElements.VisualElement;
#else
using UnityEngine.Experimental.UIElements;
#endif

namespace Unity.InteractiveTutorials
{
    public static class MaskingManager
    {
        internal static bool IsMasked(GUIViewProxy view, List<Rect> rects)
        {
            rects.Clear();

            MaskViewData maskViewData;
            if (s_UnmaskedViews.TryGetValue(view, out maskViewData))
            {
                var rectList = maskViewData.rects;
                rects.AddRange(rectList);
                return false;
            }
            return true;
        }

        internal static bool IsHighlighted(GUIViewProxy view, List<Rect> rects)
        {
            rects.Clear();
            MaskViewData maskViewData;

            if (!s_HighlightedViews.TryGetValue(view, out maskViewData))
                return false;
            var rectList = maskViewData.rects;
            rects.AddRange(rectList);
            return true;
        }

        static GUIViewProxyComparer s_GUIViewProxyComparer = new GUIViewProxyComparer();

        private static readonly Dictionary<GUIViewProxy, MaskViewData> s_UnmaskedViews = new Dictionary<GUIViewProxy, MaskViewData>(s_GUIViewProxyComparer);
        private static readonly Dictionary<GUIViewProxy, MaskViewData> s_HighlightedViews = new Dictionary<GUIViewProxy, MaskViewData>(s_GUIViewProxyComparer);

        private static readonly List<VisualElement> s_Masks = new List<VisualElement>();
        private static readonly List<VisualElement> s_Highlighters = new List<VisualElement>();

        private static double s_LastHighlightTime;

        public static float highlightAnimationDelay { get; set; }
        public static float highlightAnimationSpeed { get; set; }

        static MaskingManager()
        {
            EditorApplication.update += delegate
                {
                    // do not animate unless enough time has passed since masking was last applied
                    var t = EditorApplication.timeSinceStartup - s_LastHighlightTime - highlightAnimationDelay;
                    if (t < 0d)
                        return;

                    var baseBorderWidth = 4.2f;
                    var borderWidthAmplitude = 2.1f;
                    var animatedBorderWidth = Mathf.Cos((float)t * highlightAnimationSpeed) * borderWidthAmplitude + baseBorderWidth;
                    foreach (var highlighter in s_Highlighters)
                    {
                        if (highlighter == null)
                            continue;

                        highlighter.style.borderLeftWidth = animatedBorderWidth;
                        highlighter.style.borderRightWidth = animatedBorderWidth;
                        highlighter.style.borderTopWidth = animatedBorderWidth;
                        highlighter.style.borderBottomWidth = animatedBorderWidth;
                    }
                    foreach (var view in s_HighlightedViews)
                    {
                        if (view.Key.isValid)
                            view.Key.Repaint();
                    }
                };
        }

        public static void Unmask()
        {
            foreach (var mask in s_Masks)
            {
                if (mask != null && UIElementsHelper.GetParent(mask) != null)
                    UIElementsHelper.Remove(UIElementsHelper.GetParent(mask), mask);
            }
            s_Masks.Clear();
            foreach (var highlighter in s_Highlighters)
            {
                if (highlighter != null && UIElementsHelper.GetParent(highlighter) != null)
                    UIElementsHelper.Remove(UIElementsHelper.GetParent(highlighter), highlighter);
            }
            s_Highlighters.Clear();
        }

        private static void CopyMaskData(UnmaskedView.MaskData maskData, Dictionary<GUIViewProxy, MaskViewData> viewsAndResources)
        {
            viewsAndResources.Clear();
            foreach (var unmaskedView in maskData.m_MaskData)
            {
                if (unmaskedView.Key == null)
                    continue;
                var maskViewData = unmaskedView.Value;
                var unmaskedRegions = maskViewData.rects == null ? new List<Rect>(1) : maskViewData.rects.ToList();
                if (unmaskedRegions.Count == 0)
                    unmaskedRegions.Add(new Rect(0f, 0f, unmaskedView.Key.position.width, unmaskedView.Key.position.height));
                viewsAndResources[unmaskedView.Key] = new MaskViewData() { maskType = maskViewData.maskType, rects = unmaskedRegions };
            }
        }

        public static void AddMaskToView(GUIViewProxy view, VisualElement child)
        {
            if (view.IsDockedToEditor())
            {
                UIElementsHelper.Add(UIElementsHelper.GetVisualTree(view), child);
            }
            else
            {
                var viewVisualElement = UIElementsHelper.GetVisualTree(view);

                Debug.Assert(viewVisualElement.Children().Count() == 2
                    && viewVisualElement.Children().Count(viewChild => viewChild is IMGUIContainer) == 1,
                    "Could not find the expected VisualElement structure");

                foreach (var visualElement in viewVisualElement.Children())
                {
                    if (!(visualElement is IMGUIContainer))
                    {
                        UIElementsHelper.Add(visualElement, child);
                        break;
                    }
                }
            }
        }

        public static void Mask(
            UnmaskedView.MaskData unmaskedViewsAndRegionsMaskData, Color maskColor,
            UnmaskedView.MaskData highlightedRegionsMaskData, Color highlightColor, Color blockedInteractionsColor, float highlightThickness
            )
        {
            Unmask();

            CopyMaskData(unmaskedViewsAndRegionsMaskData, s_UnmaskedViews);
            CopyMaskData(highlightedRegionsMaskData, s_HighlightedViews);

            List<GUIViewProxy> views = new List<GUIViewProxy>();
            GUIViewDebuggerHelperProxy.GetViews(views);

            foreach (var view in views)
            {
                if (!view.isValid)
                    continue;

                MaskViewData maskViewData;

                var viewRect =  new Rect(0, 0, view.position.width, view.position.height);

                // mask everything except the unmasked view rects
                if (s_UnmaskedViews.TryGetValue(view, out maskViewData))
                {
                    List<Rect> rects = maskViewData.rects;
                    var maskedRects = GetNegativeSpaceRects(viewRect, rects);
                    foreach (var rect in maskedRects)
                    {
                        var mask = new VisualElement();
                        mask.style.backgroundColor = maskColor;
                        mask.SetLayout(rect);
                        AddMaskToView(view, mask);
                        s_Masks.Add(mask);
                    }

                    if (maskViewData.maskType == MaskType.BlockInteractions)
                    {
                        foreach (var rect in rects)
                        {
                            var mask = new VisualElement();
                            mask.style.backgroundColor = blockedInteractionsColor;
                            mask.SetLayout(rect);
                            AddMaskToView(view, mask);
                            s_Masks.Add(mask);
                        }
                    }
                }
                // mask the whole view
                else
                {
                    var mask = new VisualElement();
                    mask.style.backgroundColor = maskColor;
                    mask.SetLayout(viewRect);
                    AddMaskToView(view, mask);
                    s_Masks.Add(mask);
                }

                if (s_HighlightedViews.TryGetValue(view, out maskViewData))
                {
                    var rects = maskViewData.rects;
                    // unclip highlight to apply as "outer stroke" if it is being applied to some control(s) in the view
                    var unclip = rects.Count > 1 || rects[0] != viewRect;
                    var borderRadius = 5.0f;
                    foreach (var rect in rects)
                    {
                        var highlighter = new VisualElement();
#if UNITY_2019_3_OR_NEWER
                        highlighter.style.borderLeftColor = highlightColor;
                        highlighter.style.borderRightColor = highlightColor;
                        highlighter.style.borderTopColor = highlightColor;
                        highlighter.style.borderBottomColor = highlightColor;
#else
                        highlighter.style.borderColor = highlightColor;
#endif
                        highlighter.style.borderLeftWidth = highlightThickness;
                        highlighter.style.borderRightWidth = highlightThickness;
                        highlighter.style.borderTopWidth = highlightThickness;
                        highlighter.style.borderBottomWidth = highlightThickness;

                        highlighter.style.borderBottomLeftRadius = borderRadius;
                        highlighter.style.borderTopLeftRadius = borderRadius;
                        highlighter.style.borderBottomRightRadius = borderRadius;
                        highlighter.style.borderTopRightRadius = borderRadius;

                        highlighter.pickingMode = PickingMode.Ignore;
                        var layout = rect;
                        if (unclip)
                        {
                            layout.xMin -= highlightThickness;
                            layout.xMax += highlightThickness;
                            layout.yMin -= highlightThickness;
                            layout.yMax += highlightThickness;
                        }
                        highlighter.SetLayout(layout);
                        UIElementsHelper.Add(UIElementsHelper.GetVisualTree(view), highlighter);
                        s_Highlighters.Add(highlighter);
                    }
                }
            }

            s_LastHighlightTime = EditorApplication.timeSinceStartup;
        }

        static readonly HashSet<float> s_YCoords = new HashSet<float>();
        static readonly HashSet<float> s_XCoords = new HashSet<float>();

        static readonly List<float> s_SortedYCoords = new List<float>();
        static readonly List<float> s_SortedXCoords = new List<float>();

        internal static List<Rect> GetNegativeSpaceRects(Rect viewRect, List<Rect> positiveSpaceRects)
        {
            //TODO maybe its okay to round to int?

            s_YCoords.Clear();
            s_XCoords.Clear();

            for (int i = 0; i < positiveSpaceRects.Count; i++)
            {
                var hole = positiveSpaceRects[i];
                s_YCoords.Add(hole.y);
                s_YCoords.Add(hole.yMax);
                s_XCoords.Add(hole.x);
                s_XCoords.Add(hole.xMax);
            }

            s_YCoords.Add(0);
            s_YCoords.Add(viewRect.height);

            s_XCoords.Add(0);
            s_XCoords.Add(viewRect.width);

            s_SortedYCoords.Clear();
            s_SortedXCoords.Clear();

            s_SortedYCoords.AddRange(s_YCoords);
            s_SortedXCoords.AddRange(s_XCoords);

            s_SortedYCoords.Sort();
            s_SortedXCoords.Sort();

            var filledRects = new List<Rect>();

            for (var i = 1; i < s_SortedYCoords.Count; ++i)
            {
                var minY = s_SortedYCoords[i - 1];
                var maxY = s_SortedYCoords[i];
                var midY = (maxY + minY) / 2;
                var workingRect = new Rect(s_SortedXCoords[0], minY, 0, (maxY - minY));

                for (var j = 1; j < s_SortedXCoords.Count; ++j)
                {
                    var minX = s_SortedXCoords[j - 1];
                    var maxX = s_SortedXCoords[j];

                    var midX = (maxX + minX) / 2;


                    var potentialHole = positiveSpaceRects.Find((hole) => { return hole.Contains(new Vector2(midX, midY)); });
                    var cellIsHole = potentialHole.width > 0 && potentialHole.height > 0;

                    if (cellIsHole)
                    {
                        if (workingRect.width > 0 && workingRect.height > 0)
                            filledRects.Add(workingRect);

                        workingRect.x = maxX;
                        workingRect.xMax = maxX;
                    }
                    else
                    {
                        workingRect.xMax = maxX;
                    }
                }

                if (workingRect.width > 0 && workingRect.height > 0)
                    filledRects.Add(workingRect);
            }

            return filledRects;
        }
    }
}
