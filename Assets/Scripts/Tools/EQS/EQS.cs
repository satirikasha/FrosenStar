using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tools.EQS {


    public static class EQS {

        private static HashSet<EQSItem> _EQSItems = new HashSet<EQSItem>();

        public static void RegisterItem(EQSItem item) {
            _EQSItems.Add(item);
        }

        public static void UnregisterItem(EQSItem item) {
            _EQSItems.Remove(item);
        }

        public static IEnumerable<EQSItem> GetItems(float radius) {
            var r2 = radius * radius;
            return _EQSItems.Where(_ => {
                _.Delta = _.transform.position - PlayerController.LocalPlayer.Position;
                return _.Delta.sqrMagnitude < r2;
            });
        }

        public static IEnumerable<EQSItem> GetItems(float radius, bool visible) {
            var r2 = radius * radius;
            return _EQSItems.Where(_ => {
                _.Delta = _.transform.position - PlayerController.LocalPlayer.Position;
                return _.Visible == visible && _.Delta.sqrMagnitude < r2;
            });
        }
    }
}
