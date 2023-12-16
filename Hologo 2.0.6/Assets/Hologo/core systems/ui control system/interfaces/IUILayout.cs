

// interface to handle layout changes

public interface IUILayout
{

    void ChangeLayout(bool isLandscape, float UIwidth, float BodyWidth, float UIHeight);
}

namespace Hologo2
{
    /// <summary>
    /// Created by: Hamid (hamidibrahim@gmail.com) - 25 August 2019
    /// Modified by: 
    /// layout out interface
    /// </summary>
    public interface ILayoutUI
    {
        void changeLayout(deviceOrientation orientation, float UIwidth, float BodyWidth, float UIHeight);

        void setCanvasScaler(float scaler);

        void setMargins(float titleMargin, float bodyHeight, float footerMargin, float rightMargin, float leftMargin);
    }
}

