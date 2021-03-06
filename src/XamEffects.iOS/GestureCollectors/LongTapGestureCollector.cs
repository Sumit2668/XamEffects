﻿using System;
using System.Collections.Generic;
using UIKit;

namespace XamEffects.iOS.GestureCollectors
{
    public static class LongTapGestureCollector
    {
        private static Dictionary<UIView, GestureActionsContainer> Collection { get; } = new Dictionary<UIView, GestureActionsContainer>();

        public static void Add(UIView view, Action<UIGestureRecognizerState> action)
        {
            if (Collection.ContainsKey(view))
            {
                Collection[view].Actions.Add(action);
            }
            else
            {
                var gest = new UILongPressGestureRecognizer(ActionActivator);
                Collection.Add(view, new GestureActionsContainer
                {
                    Recognizer = gest,
                    Actions = new List<Action<UIGestureRecognizerState>>
                    {
                        action
                    }
                });
                view.AddGestureRecognizer(gest);
            }
        }

        public static void Delete(UIView view, Action<UIGestureRecognizerState> action)
        {
            if (Collection.ContainsKey(view))
            {
                var ci = Collection[view];
                ci.Actions.Remove(action);
                if (ci.Actions.Count == 0)
                {
                    view.RemoveGestureRecognizer(ci.Recognizer);
                    ci.Recognizer.Dispose();
                    ci.Recognizer = null;
                    ci.Actions = null;
                    Collection.Remove(view);
                }
            }
        }

        private static void ActionActivator(UILongPressGestureRecognizer uiLongPressGestureRecognizer)
        {
            if (Collection.ContainsKey(uiLongPressGestureRecognizer.View))
            {
                foreach (var valueAction in Collection[uiLongPressGestureRecognizer.View].Actions)
                {
                    valueAction?.Invoke(uiLongPressGestureRecognizer.State);
                }
            }
        }

        private class GestureActionsContainer
        {
            public UIGestureRecognizer Recognizer { get; set; }

            public List<Action<UIGestureRecognizerState>> Actions { get; set; }
        }

        public static void Init()
        {

        }
    }
}