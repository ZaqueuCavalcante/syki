import 'package:get/get.dart';
import 'package:flutter/material.dart';
import 'package:app/constants/syki_sizes.dart';
import 'package:app/utils/syki_functions.dart';
import 'package:app/constants/syki_images.dart';
import 'package:app/constants/syki_colors.dart';
import 'package:smooth_page_indicator/smooth_page_indicator.dart';
import 'package:app/features/student/onboarding/onboarding_step.dart';
import 'package:app/features/student/onboarding/onboarding_controller.dart';

class OnboardingPage extends StatelessWidget {
  const OnboardingPage({super.key});

  @override
  Widget build(BuildContext context) {
    final controller = Get.put(OnboardingController());

    return Scaffold(
      body: Stack(
        children: [
          /// Horizontal Scrollable GIFs
          PageView(
            controller: controller.pageController,
            onPageChanged: controller.updatePageIndicator,
            children: [
              OnboardingStep(
                image: SykiImages.onboardingCourse,
                title: 'Acompanhe seu progresso',
                subTitle:
                    'Acesse facilmente todas as informações sobre seu curso.',
              ),
              OnboardingStep(
                image: SykiImages.onboardingActivities,
                title: 'Faça as atividades',
                subTitle: 'Realize a entrega de trabalhos no próprio app.',
              ),
              OnboardingStep(
                image: SykiImages.onboardingCalendar,
                title: 'Organize sua agenda',
                subTitle:
                    'Veja todas as datas importantes e pendências de entrega.',
              )
            ],
          ),

          /// Dot Navigation SmoothPageIndicator
          Positioned(
            left: SykiSizes.defaultSpace,
            bottom: SykiFunctions.bottomNavigationBarHeight() + 25,
            child: SmoothPageIndicator(
              count: 3,
              controller: controller.pageController,
              onDotClicked: controller.dotNavigationClick,
              effect: ExpandingDotsEffect(
                activeDotColor: SykiFunctions.isDarkMode(context)
                    ? SykiColors.light
                    : SykiColors.dark,
                dotHeight: 6,
              ),
            ),
          ),

          /// Circular Button
          Positioned(
            right: SykiSizes.defaultSpace,
            bottom: SykiFunctions.bottomNavigationBarHeight(),
            child: ElevatedButton(
              onPressed: () => controller.nextStep(),
              style: ElevatedButton.styleFrom(
                shape: CircleBorder(),
                backgroundColor: SykiColors.primary,
              ),
              child: Icon(Icons.chevron_right_rounded),
            ),
          )
        ],
      ),
    );
  }
}
