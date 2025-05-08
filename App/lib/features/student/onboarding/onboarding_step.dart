import 'package:flutter/material.dart';
import 'package:app/constants/syki_sizes.dart';
import 'package:app/utils/syki_functions.dart';

class OnboardingStep extends StatelessWidget {
  const OnboardingStep({
    super.key,
    required this.image,
    required this.title,
    required this.subTitle,
  });

  final String image, title, subTitle;

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.all(SykiSizes.defaultSpace),
      child: Column(
        children: [
          Image(
            width: SykiFunctions.screenWidth() * 0.8,
            height: SykiFunctions.screenHeight() * 0.6,
            image: AssetImage(image),
          ),
          Text(
            title,
            style: Theme.of(context).textTheme.headlineMedium,
            textAlign: TextAlign.center,
          ),
          const SizedBox(
            height: SykiSizes.spaceBtwItems,
          ),
          Text(
            subTitle,
            style: Theme.of(context).textTheme.bodyMedium,
            textAlign: TextAlign.center,
          ),
        ],
      ),
    );
  }
}
