   M S B A M L   `   `   `  ����I  EAppQLQuanCaPhe, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:AppQLQuanCaPheAppQLQuanCaPhe  5   	   (    "AppQLQuanCaPhe.ProductModuleWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   (localclr-namespace:AppQLQuanCaPhe  5   	     � Title$  ProductModuleWindow��5	   	   $	��450��6%   $	��800��62    � WindowStyle$
 None=�6>    � 
ResizeMode$ NoResize=�6R   .��5
      � .+�5   
   �� 
���   �$	��0.5q�6&    �� CornerRadius$ 20��6<   ._�5      �  � RowDefinitions 5      �� 5      $��1* �6$   62   �� 5      $��8* �6$   62   .+�5      � $��0��6    � ColumnDefinitions 5      �� 5      $	��17* �6+   �� 5      $��3* �6+   .+�5      �� ���#AC3B61$��2��66   �� 5      $��Thông tin sản phẩm��6    $��20(�6:   $
��Bold"�6H   $��	309,0,0,0q�6Z   $��Center=�6m   $
��Left=�6�   ���#EDC7B76�   5      � 5      $��1��6    5      �� 5      6-   �� 5      6-   .+�5      � $��0��6    5      �� 5       $��2* �6,   6:   �� 5!      $��2* �6,   6:   �� 5"      $��2* �6,   6:   �� 5#      $��2* �6,   6:   �� 5$      $��2* �6,   6:   �� 5%      $
��1.5* �6,   6<   .+�5'      � -   5         �Name$
 tbID��5'      $��0��6-   .+�5(      �� $��Mã sản phẩm:��6(   $��15(�6<   $��10,0,0,0q�6J   $��Center=�6\   $
��Left=�6w   6�   �� 5)      -   5         ��Name$ tbID1��5)   &   $��15(�66   $��	120 0 0 0q�6D   $��Center=�6W   $
��Left=�6r    �� TextWrapping$
 Wrap=�6�   $��Mã tự động��6�   $	��150��6�   {�.  6�   5*      � 5+      $��1��6   .+�5,      �� $��Tên sản phẩm:��6(   $��15(�6=   $��10,0,0,0q�6L   $��Center=�6^   $
��Left=�6y   6�   �� 5-      -   5        $ tbtsp��5-   &   $��15(�66   $��	120 0 0 0q�6D   $��Center=�6W   $
��Left=�6r   $
 Wrap=�6�   $	��150��6�   5.      � 5/      $��2��6   .+�50      �� -   5        	 ��Name$	 cblsp��50   '   )  #MaterialDesignThemes.Wpf.HintAssist
   Hint$
 Loại sản phẩm��51      $
��Left=�6L   $��35��6g   $��	120,0,0,0q�6s   $	��150��6�   53      �� 54      $��Loại sản phẩm:��6(   $��15(�6>   $��10,0,0,0q�6L   $��Center=�6^   $
��Left=�6y   6�   55      � 56      $��3��6   .+�57      �� -   5        $
	 cbkt��57   '   $
  ��58      $
��Left=�6?   $��35��6Z   $��	120,0,0,0q�6f   $	��150��6y   .@�5:   "   �� $��No Size��5;   %   �� 5<   "   $��S��5=   %   �� 5>   "   $��M��5?   %   �� 5@   "   $��L��5A   &   5B      �� 5C      $��Kích thước:��6(   $��15(�6;   $��10,0,0,0q�6I   $��Center=�6[   $
��Left=�6v   6�   5D      � 5E      $��4��6   .+�5F      �� -   5        $ cbgia��5F   &   $��15(�66   $��	120 0 0 0q�6D   $��Center=�6W   $
��Left=�6r   $	��150��6�   �� 5G      $��
Giá bán:��6(   $��15(�68   $��10,0,0,0q�6G   $��Center=�6Y   $
��Left=�6t   6�   5H      � 5I      $��5��6   5K      5L      � 5M      $��1��6    5N      �� 5O      $	��10* �6,   6;   �� 5P      $
��1.5* �6,   6<   .+�5R      � $��0��6   .+�5S      �� -   5        $��	btnTaiLen��5S   %   $	��144��5T      $��
Tải lên��5U       #  MaterialDesignRaisedLightButton#��[  5W       � ToolTip$4 .Resource name: MaterialDesignRaisedLightButton��5X      ���#AC3B61���#EDC7B7$��Center=�5[      $��0,170,0,100q�6:   {�5V      �� *DataContext.ControlsEnabled+ �� RelativeSource �� *FindAncestor+ �� AncestorType$ WindowL�5[   O   �� 5]      -   5         ��Name$
 lbID��5]   (   $y�	Collapsed=�66   $��Center=�6M   $��0,75,0,0q�6j   $��3��6|   $	��Top=�6�    J� RenderTransformOrigin$ -1.142,0.517&�6�   $��22��6�   $��2��6�   $��49��6�   5^      �� 5_      -	   5         ��Name$ txtimg��5_   $   $	��150��65   $	��150��6B   $��125 25 125 150q�6N   $��Stretch=�6f   $��Stretch=�6�   $��/icons8-image-96.png��6�    J� OpacityMask 5`   "   �� 5a   &    �� ImageSource$ /icons8-image-96.png��61    j� Stretch$ UniformToFill=�6T   5c      5d      � 5e      $��-76,347,0,0q�6   $��2��64   .+�5f      �� -
   5         ��Name$ 
btnCapNhat��5f   %   $	��145��5g   !   $��Cập Nhật��5h   !   #��[  5j   !   ���#AC3B61���#EDC7B7$
��Left=�5m   !   $��	0,2,170,2q�6<   $4 .Resource name: MaterialDesignRaisedLightButton��5n   !   {�5i   !   �� *DataContext.ControlsEnabled+ �� *FindAncestor+$ WindowL�5n   Z   �� 5p      -   5        $ btnXoa��5p   %   $	��145��5q   !   $
��Xóa��5r   !   #��[  5t   !   ���#AC3B61���#EDC7B7$��Right=�5w   !   $��	0,2,170,2q�6=   $4 .Resource name: MaterialDesignRaisedLightButton��5x   !   {�5s   !   �� *DataContext.ControlsEnabled+ �� *FindAncestor+$ WindowL�5x   Z   �� 5z      -   5        $ btnLuu��5z   %   $	��145��5{   !   $
��Lưu��5|   !   #��[  5~   !   ���#AC3B61���#EDC7B7$��Right=�5�   !   $��	0,2,170,2q�6=   $4 .Resource name: MaterialDesignRaisedLightButton��5�   !   {�5}   !   �� *DataContext.ControlsEnabled+ �� *FindAncestor+$ WindowL�5�   Z   �� 5�      -   5        $ btnThoat��5�   %   $	��145��5�   !   $��Hủy��5�   !   #��[  5�   !   $4 .Resource name: MaterialDesignRaisedLightButton��5�   !   ���#AC3B61���#EDC7B7$��Right=�5�   !   $��0,2,10,2q�6=   {�5�   !   �� *DataContext.ControlsEnabled+ �� *FindAncestor+$ WindowL�5�   O   5�      5�      5�      5�      5�      5�      5�      