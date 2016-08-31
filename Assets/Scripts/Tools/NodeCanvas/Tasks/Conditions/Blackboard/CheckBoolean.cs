using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Conditions{

	[Category("✫ Blackboard")]
	public class CheckBoolean : ConditionTask{

		[BlackboardOnly]
		public BBParameter<bool> valueA;
		public BBParameter<bool> valueB = true;

		protected override string info{
			get {return valueA + " == " + valueB;}
		}

		protected override bool OnCheck(){
            //debug.Log(valueA.name);
            if (valueA.name == "IsSeeStunnedBody") {
                //Debug.Log("="+valueA.value);

                if (valueA.value) {
                    //Debug.Log("Check disc stunned=" + valueA.value);
                }
            }

			return valueA.value == valueB.value;
		}
	}
}