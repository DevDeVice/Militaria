   M S B A M L   `   `   `  ����  
Militaria2    Militaria2.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Title$  Product Aggregator��5   	   $	��450��6$   $	��800��61   .��5      �  � RowDefinitions 5	   
   �� 5
      $��* �6   �� 5      $
��Auto �6   .+�5   
   &   System.Windows.Controls.DataGrid  $��0��6      AutoGenerateColumns$ False��6A      CanUserAddRows$ False��6]      
IsReadOnly$ false��6t   ��6    �� *
Products+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ 50 6H   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *Id+6H     5      $	 EAN��6%   $	 100 6J    62   �� *EAN+6J     5      $	 SKU��6%   $	 100 6J    62   �� *SKU+6J     5      $
 Name��6%   $	 200 6L    63   �� *Name+6L     5      $ Description��6%   $	 300 6Z    6:   �� *Description+6Z     5      $ Gross Price��6%   $	 100 6Y    6:   �� *
GrossPrice+6Y     5      $ 	Net Price��6%   $	 100 6U    68   �� *
NetPrice+6U     5      $	 VAT��6%   $ 50 6J    62   �� *VAT+6J     5      $ Stock Quantity��6%   $	 100 6_    6=   �� *StockQuantity+6_   4  .System.Windows.Controls.DataGridTemplateColumn  5      $ Image��6)   $	 100 68   	   CellTemplate	 5      �� 5      .R�5      �� $��50��6A   $��50��6M   ��6$   �� *	Images[0]+6M   5      5        5       $ Variants��6)   $	 300 6;   	 5!      �� 5"      .R�5#      �� ��6+   �� *
Variants+��5$   "   �� 5%   &   .R�5&   *   �� 
 �� Orientation$
 
Horizontal=�65   .+�5'   .   �� $��5q�6N   ��68   �� *Code+6N   �� 5(   .   $	�� - ��68   �� 5)   .   $��5q�6R   ��68   �� *
Quantity+6R   5*   +   5+   '   5-      5.      50      4  .System.Windows.Controls.DataGridCheckBoxColumn  51      $ Flag for Offer��6)   $	 100 6e    6A   �� *FlaggedForOffer+6e   53      �� 55   
   $��Load Products��6   $��Center=�56      $��Bottom=�6.   $	��100��57      $��30��6   $��1��6)   $
��0,10q�66   ��55   )   �� *LoadProductsCommand+57   6   58      59      