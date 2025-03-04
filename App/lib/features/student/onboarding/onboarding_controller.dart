import 'package:app/pages/login_page.dart';
import 'package:get/get.dart';
import 'package:flutter/material.dart';

class OnboardingController extends GetxController {
  static OnboardingController get instance => Get.find();

  final pageController = PageController();
  Rx<int> currentStepIndex = 0.obs;

  void updatePageIndicator(int index) => currentStepIndex.value = index;

  void dotNavigationClick(int index) {
    currentStepIndex.value = index;
    pageController.jumpTo(index.toDouble());
  }

  void nextStep() {
    if (currentStepIndex.value == 2) {
      Get.to(LoginPage(
        goToRegisterPage: () {},
      ));
    } else {
      int step = currentStepIndex.value + 1;
      pageController.jumpToPage(step);
    }
  }

  void skip() {
    currentStepIndex.value = 2;
    pageController.jumpToPage(2);
  }
}
