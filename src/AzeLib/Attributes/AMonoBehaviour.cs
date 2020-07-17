using System.Reflection;

namespace AzeLib.Attributes
{
    public class AMonoBehaviour : KMonoBehaviour
    {
        //TODO: Optimize this similarily to the base game
        public override void OnSpawn()
        {
            base.OnSpawn();

            foreach (var fieldInfo in GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                foreach (var obj in fieldInfo.GetCustomAttributes(false))
                {
                    if(obj.GetType() == typeof(MyIntGetAttribute))
                    {
                        fieldInfo.SetValue(this, GetComponent(fieldInfo.FieldType));
                    }
                }
            }
        }
    }
}
