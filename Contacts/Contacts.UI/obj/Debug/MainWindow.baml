   M S B A M L   `   `   `  ����F  BContacts.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:Contacts.UIContacts.UI  5   	       Contacts.UI.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:Contacts.UI     5   	     � Title$  Contacts��5   	   $	��450��6   $	��800��6'   .��5	      � $��10q�6    � RowDefinitions 5
   
   �� 5      $	��300 �6   6+   �� 5      $��50 �6   6*   �� 5      $
��Auto �6   6,   .+�5   
   &   System.Windows.Controls.DataGrid  -   5        $��
dgContacts��5         AutoGenerateColumns .     
IsReadOnly . $��0��5         CanUserAddRows .  ��5      �� *
Contacts+ �� SelectedItem 64   �� *SelectedPerson+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ 
First Name��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$	 150 6W   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 69   �� *	FirstName+6W     5      $ 	Last Name��6%   $	 150 6U   	 68   �� *
LastName+6U   
   RowDetailsTemplate
 5      �� 5      .R�5          Name$ dgAddresses��6    .  .  ��64   �� *	Addresses+ 5        5      $ Street��61   $	 120 6\   	 6A   �� *Street+6\     5      $
 City��61   $	 120 6X   	 6?   �� *City+6X     5      $ State��61   $	 120 6Z   	 6@   �� *State+6Z     5       $ Postal code��61   $	 120 6e   	 6F   �� *
PostalCode+6e   5"      5#      5%      �� 5&   
   $��1��6   ��6!   ��  �� Path$ InfoMessage� �� UpdateSourceTrigger$ PropertyChanged=�6j   � 5'   
   $��2��6    � ColumnDefinitions 5(      �� 5)      $	��120 �6#   61   �� 5*      $	��120 �6#   61   �� 5+      $	��120 �6#   61   �� 5,      $	��200 �6#   61   .+�5.      �� -   5        $��	btnAddNew��5.      $��0��6&   $��Add New��66   $��24��6j   $��95��6v   ��6H   �� *AddNewCommand+6�   �� 5/      -   5        $��btnEdit��5/      $��1��6$   $
��Edit��64   $��24��6c   $��95��6o   ��6C   �� *EditCommand+6|   �� 50      -   5        $��	btnDelete��50      $��2��6&   $��Delete��66   $��24��6i   $��95��6u   ��6G   �� *DeleteCommand+6�   51      53      54      