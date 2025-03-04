import 'package:get/get.dart';
import 'package:app/configs/env.dart';
import 'package:flutter/material.dart';
import 'package:app/configs/http_configs.dart';
import 'package:app/themes/syki_app_theme.dart';
import 'package:app/configs/services_configs.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:app/features/student/onboarding/onboarding_page.dart';

Future<void> main() async {
  await dotenv.load(fileName: Env.fileName);

  addServicesConfigs();
  addHttpConfigs();

  // AuthProvider
  //https://medium.com/@areesh-ali/building-a-secure-flutter-app-with-jwt-and-apis-e22ade2b2d5f,
  runApp(const SykiApp());
}

class SykiApp extends StatelessWidget {
  const SykiApp({super.key});

  @override
  Widget build(BuildContext context) {
    return GetMaterialApp(
      debugShowCheckedModeBanner: false,
      home: const OnboardingPage(),
      themeMode: ThemeMode.system,
      theme: SykiAppTheme.lightTheme,
      darkTheme: SykiAppTheme.darkTheme,
    );
  }
}
