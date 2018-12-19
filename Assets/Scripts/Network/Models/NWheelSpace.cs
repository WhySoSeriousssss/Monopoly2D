
public class NWheelSpace : NSpace {

    public override void StepOn(NPlayer player)
    {
        NSpinWheelManager.instance.InvokeWheel(player);
    }
}
