   M S B A M L   `   `   `  ����  AppQLQuanCaPhe(    "AppQLQuanCaPhe.InvoiceDetailWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Chi Tiết Hóa Đơn��5   	   $	��650��6"   $	��600��6/    � WindowStyle$
 None=�6;    � AllowsTransparency . .��5      � ���#EEE2DC � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $
��Auto �6   �� 5
      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��Chi Tiết Hóa Đơn��6   $��24(�6,   $
��Bold"�6:   $��10q�6L   $��Center=�6X   $��0��6u   �� 5   
   $��1��6   $��10q�6"   .+�5      �� $��Mã Hóa Đơn:��6   $
��Bold"�6+   �� 5      -   5         ��Name$ TextMaHoaDon��5       �� 
IsReadOnly . $��5q�6>   �� 5      $��
Tên Bàn:��6   $
��Bold"�6(   $��0,10,0,0q�6:   �� 5      -   5        $ 
TextTenBan��5       . $��5q�6<   5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ detailDataGrid��5      $��2��6+      AutoGenerateColumns .     CanUserAddRows .  $��10q�6k   	   
IsReadOnly	 . 
   Columns
 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Tên Sản Phẩm��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ 2* 6Z   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6;   �� *
TenSanPham+6Z     5       $
 Size��6%   $ * 6L    63   �� *Size+6L     5!      $ Số Lượng��6%   $
 1.5* 6S    67   �� *	SoLuong+6S     5"      $
 Giá��6%   $ * 6[    62   �� *Gia+ �� StringFormat$ N0��6[     5#      $ Tổng��6%   $ * 6]    63   �� *Tong+$ N0��6]   5%      �� 5(   
   $��3��6    �� Orientation$ 
Horizontal=�6"   $��10q�6;   .+�5)      �� $ ��Tổng số lượng món:��6   $��16(�62   $��Center=�6@   �� 5*      -   5         ��Name$ totalQuantityTextBlock��5*      $��16(�68   $��10,0,0,0q�6F   $��Center=�6X   ���#AC3B616s   5+      �� 5.   
   $��4��6   $ 
Horizontal=�6"   $��10q�6;   .+�5/      �� $��Tổng tiền hàng:��6   $��16(�6/   $��Center=�6=   �� 50      -   5        $ totalAmountTextBlock��50      $��16(�66   $��10,0,0,0q�6D   $��Center=�6V   ���#AC3B616q   51      �� 54   
   $��5��6   $ 
Horizontal=�6"   $��10q�6;   .+�55      �� $$��Tổng tiền sau giảm giá:��6   $��16(�67   $��Center=�6E   �� 56      -   5        $ discountedAmountTextBlock��56      $��16(�6;   $��10,0,0,0q�6I   $��Center=�6[   ���#AC3B616v   57      � 58   
   $��6��6   $��Right=�6   $��10q�68   .+�59      �� $ 
Horizontal=�6   .+�5:      �� -   5        $��Hủy đơn��5:      ���#AC3B61���#EDC7B7$��0,0,5,0q�6j   �� 5;      -   5        $��
Quay lại��5;      ���#AC3B61���#EDC7B76V   5<      5=      5?      5@      