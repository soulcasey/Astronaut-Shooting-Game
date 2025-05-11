public class SpikeObjectPool : ObjectPool<Spike>
{
    protected override int InitialSize => 10;
}