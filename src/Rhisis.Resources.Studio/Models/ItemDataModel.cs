using Rhisis.Game.Common.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhisis.Resources.Studio.Models
{
    public class ItemDataModel
    {
        public ItemDataModel(ItemData itemData)
        {
            Id = itemData.Id;
            Name = itemData.Name;
            Level = itemData.Level;
            Job = itemData.ItemJob.ToString();
            KindOne = itemData.ItemKind1.ToString();
            KindTwo = itemData.ItemKind2.ToString();
            KindThree = itemData.ItemKind3.ToString();
            Parts = itemData.Parts.ToString();
        }

        public int Id { get; }
        public string Name { get; }
        public int Level { get; }
        public string Job { get; }
        public string KindOne { get; }
        public string KindTwo { get; }
        public string KindThree { get; }
        public string Parts { get; }
    }
}
