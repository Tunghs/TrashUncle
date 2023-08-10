using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace TrashUncle
{
    internal class PopupManage
    {
        private Window window;
        private Popup popup;

        public PopupManage(Window window, Popup popup)
        {
            this.window = window;
            this.popup = popup;
        }

        public CustomPopupPlacement[] GetPopupLocation(Size popupSize, Size targetSize, Point offset)
        {
            var centerX = 0.0;
            if (window.Left - ((260 / 2) + 20) < 0)
            {
                centerX = (window.Width / 2) - 10;
                popup.FlowDirection = FlowDirection.LeftToRight;
                // ShowMessage($"왼쪽입니다.{centerX}");
            }
            else
            {
                centerX = (window.Width / 2) - 500;

                popup.FlowDirection = FlowDirection.RightToLeft;
                /// ShowMessage($"오른쪽입니다.{centerX}");
            }

            CustomPopupPlacement placement1 = new CustomPopupPlacement(new Point(centerX, 55), PopupPrimaryAxis.Vertical);
            CustomPopupPlacement placement2 = new CustomPopupPlacement(new Point(0, 55), PopupPrimaryAxis.Horizontal);

            CustomPopupPlacement[] placementArray = new CustomPopupPlacement[] { placement1, placement2 };

            return placementArray;
        }
    }
}
