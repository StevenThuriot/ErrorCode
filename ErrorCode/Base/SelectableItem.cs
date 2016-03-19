using System;

namespace ErrorCode.Base
{
    public abstract class SelectableItem : Notifyable
    {
        bool _isExpanded;
        protected bool _isSelected;

        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }

            set
            {
                SetValue(ref _isExpanded , value);
            }
        }

        public virtual bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                SetValue(ref _isSelected, value);
            }
        }
    }

    public abstract class SelectableItem<T> : SelectableItem
        where T : SelectableItem
    {

        public T Parent { get; }
        public SelectableItem(T parent)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            Parent = parent;
        }

        public override bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                if (SetValue(ref _isSelected, value) && value)
                {
                    Parent.IsSelected = true;
                }
            }
        }
    }
}
