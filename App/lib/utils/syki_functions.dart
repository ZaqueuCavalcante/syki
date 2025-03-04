import 'package:get/get.dart';
import 'package:flutter/material.dart';

class SykiFunctions {
  static void navigateToScreen(BuildContext context, Widget screen) {
    Navigator.push(context, MaterialPageRoute(builder: (_) => screen));
  }

  static Size screenSize() {
    return MediaQuery.of(Get.context!).size;
  }

  static double screenHeight() {
    return MediaQuery.of(Get.context!).size.height;
  }

  static double screenWidth() {
    return MediaQuery.of(Get.context!).size.width;
  }

  static double statusBarHeight() {
    return MediaQuery.of(Get.context!).padding.top;
  }

  static double keyboardHeight() {
    return MediaQuery.of(Get.context!).viewInsets.bottom;
  }

  static bool isKeyboardVisible() {
    return View.of(Get.context!).viewInsets.bottom > 0;
  }

  static bool isDarkMode(BuildContext context) {
    return Theme.of(context).brightness == Brightness.dark;
  }

  static double appBarHeight() {
    return kToolbarHeight;
  }

  static double bottomNavigationBarHeight() {
    return kBottomNavigationBarHeight;
  }
}
