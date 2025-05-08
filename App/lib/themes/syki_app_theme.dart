import 'package:flutter/material.dart';
import 'package:app/themes/syki_text_theme.dart';
import 'package:app/themes/syki_chip_theme.dart';
import 'package:app/themes/syki_app_bar_theme.dart';
import 'package:app/themes/syki_checkbox_theme.dart';
import 'package:app/themes/syki_bottom_sheet_theme.dart';
import 'package:app/themes/syki_elevated_button_theme.dart';
import 'package:app/themes/syki_outlined_button_theme.dart';
import 'package:app/themes/syki_input_decoration_theme.dart';

class SykiAppTheme {
  static ThemeData lightTheme = ThemeData(
    useMaterial3: true,
    brightness: Brightness.light,
    primaryColor: Colors.blue,
    scaffoldBackgroundColor: Colors.white,
    textTheme: SykiTextTheme.lightTextTheme,
    chipTheme: SykiChipTheme.lightChipTheme,
    appBarTheme: SykiAppBarTheme.lightAppBarTheme,
    checkboxTheme: SykiCheckboxTheme.lightCheckboxTheme,
    bottomSheetTheme: SykiBottomSheetTheme.lightBottomSheetTheme,
    elevatedButtonTheme: SykiElevatedButtonTheme.lightElevatedButtonTheme,
    outlinedButtonTheme: SykiOutlinedButtonTheme.lightOutlinedButtonTheme,
    inputDecorationTheme: SykiInputDecorationTheme.lightInputDecorationTheme,
  );

  static ThemeData darkTheme = ThemeData(
    useMaterial3: true,
    brightness: Brightness.dark,
    primaryColor: Colors.blue,
    scaffoldBackgroundColor: Colors.grey[850],
    textTheme: SykiTextTheme.darkTextTheme,
    chipTheme: SykiChipTheme.darkChipTheme,
    appBarTheme: SykiAppBarTheme.darkAppBarTheme,
    checkboxTheme: SykiCheckboxTheme.darkCheckboxTheme,
    bottomSheetTheme: SykiBottomSheetTheme.darkBottomSheetTheme,
    elevatedButtonTheme: SykiElevatedButtonTheme.darkElevatedButtonTheme,
    outlinedButtonTheme: SykiOutlinedButtonTheme.darkOutlinedButtonTheme,
    inputDecorationTheme: SykiInputDecorationTheme.darkInputDecorationTheme,
  );
}
