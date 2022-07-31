using MeiMvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace FirstDraft.ApplyDemo.ViewModels
{
    public class ApplyComboBoxViewModel : NotifyPropertyChanged
    {
        public Dictionary<GCCollectionMode,string> GCCollections { get; set; }

        private GCCollectionMode currentGCCM;

        public GCCollectionMode CurrentGCCM
        {
            get { return currentGCCM; }
            set { Set(ref currentGCCM, value); }
        }

        public ApplyComboBoxViewModel()
        {
            GCCollections = new Dictionary<GCCollectionMode, string>();
            foreach (var item in Enum.GetNames(typeof(GCCollectionMode)))
            {
                GCCollectionMode mode =(GCCollectionMode) Enum.Parse(typeof(GCCollectionMode), item);
                GCCollections.Add(mode,item);
            }
            CurrentGCCM = GCCollectionMode.Default;

            Types = new List<Type>();
            Types.Add(typeof(ApplyComboBoxViewModel));
            Types.Add(typeof(ApplyIconButtonViewModel));
            Types.Add(typeof(ApplyTextBoxViewModel));
            CurrentType = Types[0];
        }

        private Type currentType;

        public Type CurrentType
        {
            get { return currentType; }
            set { Set(ref currentType, value); }
        }


        public List<Type> Types { get; set; }
    }
}
