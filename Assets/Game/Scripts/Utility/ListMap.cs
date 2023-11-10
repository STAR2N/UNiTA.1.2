namespace Game.Utility {
    using System.Collections.Generic;

    public class ListMap<Key, Value> : Dictionary<Key, List<Value>> {
        public virtual void AddElement(Key key, Value value) {
            if (!TryGetValue(key, out var list)) {
                list = new List<Value>();
                Add(key, list);
            }

            list.Add(value);
        }

        public virtual void RemoveElement(Key key, Value value) {
            if (TryGetValue(key, out var list)) {
                list.Remove(value);
            }
        }

        public virtual bool HasElement(Key key, Value value) {
            if (TryGetValue(key, out var list )) {
                return list.Contains(value);
            }
            return false;
        }
    }
}