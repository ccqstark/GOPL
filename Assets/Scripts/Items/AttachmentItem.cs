namespace Scripts.Items
{
    public class AttachmentItem : BaseItem
    {
        public enum AttachmentType
        {
            Scope,
            Other
        }

        public AttachmentType CurrentAttachmentType;
    }
}
