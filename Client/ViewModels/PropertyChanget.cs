﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    internal abstract class PropertysChanged : INotifyPropertyChanged
    {
        //public event NotifyCollectionChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }        
        #region IDataErrorInfo
        protected Dictionary<string, string> ValidationErrors = new Dictionary<string, string>();
        public string Error => throw new NotImplementedException();
        public string this[string columnName] => ValidationErrors.ContainsKey(columnName) ? ValidationErrors[columnName] : null;
        protected bool IsValid(object obj) => ValidationErrors.Values.All(x => x == null);
        #endregion
    }
}
