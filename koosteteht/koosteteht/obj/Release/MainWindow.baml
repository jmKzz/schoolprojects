   M S B A M L   `   `   `  ���� E  Akoosteteht, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:koosteteht
koosteteht  =#clr-namespace:koosteteht.Propertieskoosteteht.Properties      koosteteht.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *localclr-namespace:koosteteht     1p#clr-namespace:koosteteht.Properties     -     � Title$  WPF Painter��$	��768��$
��1024��$	��640��$	��500��.��]� .+�o� -    o�Name$ menuTop��$	��Top=�.@�m� %   koosteteht.Properties.Resources   menuFile#��Z .@�m� -    m�Name$ MenuNew��   menuNew#��Z  m� Icon �� $��newproject.png��m� -   $ MenuOpen��   menuOpen#��Z  �� $��savebutton.png��m� -   $ MenuSave��   menuSave#��Z  �� $��savebutton.png��m� -   $ 	MenuPrint��   	menuPrint#��Z  �� $��printbutton.png���� m� -   	   menuExit#��Z	 
 m� Command
 �� *ExitCommand+m�    menuHelp#��Z .@�m� -   $ 
MenuManual��   
menuManual#��Z m� -	   $ 	MenuAbout��   	menuAbout#��Z � -
    �Name$ gridMain��.+��  � RowDefinitions �� $��* � � ColumnDefinitions �� $��* �.+��� -    ��Name$ 
inkCanvas1��$��	0,0,114,0q��� 
�������
�������$��Right=�$��3q�._��� $	��113��$
��Left=�$
��Left=�.+�d� -    d�Name$ selectButton��$��Selection tool��$��
10,10,10,0q�d� -   $ 
kumiButton��$��Eraser��$��
10,10,10,0q��� $
��Wrap=�$��Eraser size��$��Center=�$��
16,10,14,0q��� -    ��Name$ eraserSizeSlider��$��10,10q� � Maximum$ 30Q� �� IsSnapToTickEnabled .  � Value$ 5Q� �� TickFrequency$ 1Q�	� -    	�Name$ eraser1��$��	Rectangle��$��	20,10,0,5q�$
��Left=�	� -   $ eraser2��$��Ellipse��$��	20,0,0,10q�$
��Left=��� $
��Wrap=�$+��%        Stylus         (Width/Height)��$��Center=�$��0,10,0,0q��� $��	10,5,10,0q�$ 15Q� �� * DefaultDrawingAttributes.Width+ �� ElementName$ 
inkCanvas1���� $
��10,0q�$ 15Q� �� *!DefaultDrawingAttributes.Height+$ 
inkCanvas1��� -   $ 
gridColors��$��1,10,1,0q�$	��186�� �� $	��10* ��� $	��10* ��� $	��10* � �� $	��20* ��� $	��20* ��� $	��20* ��� $	��20* ��� $	��20* ��� $	��20* �.+��� -    ��Name$ buttonYellow��$��0��$��0��
��� ����� -   $ buttonGreen��$��0��$��1��
��� � ��� -   $ 
buttonBlue��$��0��$��2��
����  ��� -   $ 	buttonRed��$��1��$��0��
���  ���� -   $ buttonViolet��$��1��$��1��
�������� -   $ buttonBrown��$��1��$��2��
���**���� -   $ buttonTurqoise��$��2��$��0��
�����@��� -   $ buttonOrange��$��2��$��1��
��� ����� -   $ buttonLightBlue��$��2��$��2��
����ح��� -   $ 
buttonPink��$��3��$��0��
��������� -   $ 
buttonGold��$��3��$��1��
��� ����� -   $ buttonMintCream��$��3��$��2��
��������� -   $ buttonPurple��$��4��$��0��
���� ���� -   $ buttonDarkRed��$��4��$��1��
���  ���� -    $ 
buttonTeal��$��4��$��2��
����� ��� -!   $ 
buttonGray��$��5��$��0��
�������$
��Left=�$��37���� -"   $ buttonBlack��$��5��$��1��
���   ��� -#   $ buttonWhite��$��5��$��2��
��������� $��0,10,0,0q�$��80��.���� $��ellipsi.png���� $��0,10,0,0q�$��80��.���� $��rectangle.png��