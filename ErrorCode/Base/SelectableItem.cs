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

        public void Select()
        {
            IsSelected = true;
            IsExpanded = true;
        }
    }    
}
