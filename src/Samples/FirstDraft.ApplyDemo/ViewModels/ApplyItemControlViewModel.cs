using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace FirstDraft.ApplyDemo.ViewModels
{
    public class ApplyComboBoxViewModel : ObservableObject
    {
        public Dictionary<GCCollectionMode, string> GCCollections { get; set; }

        private GCCollectionMode currentGCCM;

        public GCCollectionMode CurrentGCCM
        {
            get { return currentGCCM; }
            set { SetProperty(ref currentGCCM, value); }
        }

        private ItemDataModel currentItem;

        public ItemDataModel CurrentItem
        {
            get { return currentItem; }
            set { SetProperty(ref currentItem, value); }
        }

        public List<ItemDataModel> Areas { get; set; }

        public Dictionary<int, ItemDataModel> AreaKVs { get; set; }

        private int currentId = 1;

        public int CurrentId
        {
            get { return currentId; }
            set { SetProperty(ref currentId, value); }
        }

        private string currentName = "北京";

        public string CurrentName
        {
            get { return currentName; }
            set { SetProperty(ref currentName, value); }
        }

        public ApplyComboBoxViewModel()
        {
            GCCollections = new Dictionary<GCCollectionMode, string>();
            foreach (var item in Enum.GetNames(typeof(GCCollectionMode)))
            {
                GCCollectionMode mode = (GCCollectionMode)Enum.Parse(typeof(GCCollectionMode), item);
                GCCollections.Add(mode, item + "模式");
            }
            CurrentGCCM = GCCollectionMode.Default;

            Areas = new List<ItemDataModel>();
            Areas.Add(new ItemDataModel() { Id = 1, Name = "北京" });
            Areas.Add(new ItemDataModel() { Id = 2, Name = "重庆" });
            Areas.Add(new ItemDataModel() { Id = 3, Name = "浙江" });
            Areas.Add(new ItemDataModel() { Id = 4, Name = "福建" });
            Areas.Add(new ItemDataModel() { Id = 5, Name = "贵州" });
            CurrentItem = Areas[0];


            AreaKVs = new Dictionary<int, ItemDataModel>();
            for (int i = 0; i < Areas.Count; i++)
            {
                AreaKVs.Add(Areas[i].Id, Areas[i]);
            }
        }


    }

    public class ItemDataModel : ObservableObject
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

}
