

public interface IPickUpAquierer
{
    public void AttemptToAquirePickUp(PickUp weaponToAquire);
    protected void AquirePickUp(PickUp weaponToAquire);
}
