namespace Engine.Core
{
    public static partial class ObjectLoader
    {
        public class KretzTag
        {
            public ushort  Group;
            public ushort  Element;

            public KretzTag(ushort tagGroup, ushort tagElement)
            {
                Group = tagGroup;
                Element = tagElement;
            }

            public override bool Equals(object obj)
            {
                var kr = (KretzTag)(obj);
                return Element == kr.Element && Group == kr.Group;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override string ToString()
            {
                var g = Group.ToString("X");
                var e = Element.ToString("X");
                return $"{g}|{e}";
            }
        }

    }
}
