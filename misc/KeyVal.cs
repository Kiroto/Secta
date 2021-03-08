public class KeyVal<Key, Val>
{
    public Key Index { get; set; }
    public Val Value { get; set; }

    public KeyVal() { }

    public KeyVal(Key key, Val val)
    {
        this.Index = key;
        this.Value = val;
    }
}