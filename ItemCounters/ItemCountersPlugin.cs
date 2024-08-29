using BepInEx;
using RoR2;
using System.Reflection;
using System.Text;
using UnityEngine.UI;

namespace ItemCounters {

    [BepInPlugin(ModGuid, "Item Counters", "1.0.4")]
    public class ItemCountersPlugin : BaseUnityPlugin {

        private const string ModGuid = "com.github.mcmrarm.itemcounters";

        private static FieldInfo scoreboardStripMasterField = typeof(RoR2.UI.ScoreboardStrip).GetField("master", BindingFlags.NonPublic | BindingFlags.Instance);
        
        public void Start() {
            On.RoR2.UI.ScoreboardStrip.UpdateItemCountText += ScoreboardStrip_UpdateItemCountText;
        }

        private void ScoreboardStrip_UpdateItemCountText(On.RoR2.UI.ScoreboardStrip.orig_UpdateItemCountText orig, RoR2.UI.ScoreboardStrip self)
        {

            if (!(self.master && self.itemInventoryDisplay && self.itemInventoryDisplay.GetTotalItemCount() != self.previousItemCount)) return;

            CharacterMaster master = self.master;
            int tier1Count = master.inventory.GetTotalItemCountOfTier(ItemTier.Tier1);
            int tier1VoidCount = master.inventory.GetTotalItemCountOfTier(ItemTier.VoidTier1);
            int tier2Count = master.inventory.GetTotalItemCountOfTier(ItemTier.Tier2);
            int tier2VoidCount = master.inventory.GetTotalItemCountOfTier(ItemTier.VoidTier2);
            int tier3Count = master.inventory.GetTotalItemCountOfTier(ItemTier.Tier3);
            int tier3VoidCount = master.inventory.GetTotalItemCountOfTier(ItemTier.VoidTier3);
            int lunarCount = master.inventory.GetTotalItemCountOfTier(ItemTier.Lunar);
            int bossCount = master.inventory.GetTotalItemCountOfTier(ItemTier.Boss);

            //Need to combine these because there's not enough space for all of them.
            int voidCount = tier1VoidCount + tier2VoidCount + tier3VoidCount;

            //Vanilla method shows hidden items.
            self.previousItemCount = tier1Count + tier2VoidCount + tier3Count + lunarCount + bossCount + voidCount;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<nobr><color=#fff>{0} (", self.itemInventoryDisplay.GetTotalItemCount());
            if (tier1Count > 0)
                sb.AppendFormat("<color=#{1}>{0}</color> ", tier1Count, ColorCatalog.GetColorHexString(ColorCatalog.ColorIndex.Tier1Item));
            if (tier2Count > 0)
                sb.AppendFormat("<color=#{1}>{0}</color> ", tier2Count, ColorCatalog.GetColorHexString(ColorCatalog.ColorIndex.Tier2Item));
            if (tier3Count > 0)
                sb.AppendFormat("<color=#{1}>{0}</color> ", tier3Count, ColorCatalog.GetColorHexString(ColorCatalog.ColorIndex.Tier3Item));
            if (lunarCount > 0)
                sb.AppendFormat("<color=#{1}>{0}</color> ", lunarCount, ColorCatalog.GetColorHexString(ColorCatalog.ColorIndex.LunarItem));
            if (bossCount > 0)
                sb.AppendFormat("<color=#{1}>{0}</color> ", bossCount, ColorCatalog.GetColorHexString(ColorCatalog.ColorIndex.BossItem));
            if (voidCount > 0)
                sb.AppendFormat("<color=#{1}>{0}</color> ",voidCount, ColorCatalog.GetColorHexString(ColorCatalog.ColorIndex.VoidItem));
            if (sb[sb.Length - 1] == ' ')
                sb[sb.Length - 1] = ')';
            else if (sb[sb.Length - 1] == '(')
                sb.Length = sb.Length - 2;
            self.itemCountText.text = sb.ToString();
        }
    }
}
