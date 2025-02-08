import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:app/pages/home_page.dart';
import 'package:app/themes/theme_provider.dart';
import 'package:app/configs/services_configs.dart';

void main() {
  addServicesConfigs();

  runApp(ChangeNotifierProvider(
    create: (context) => ThemeProvider(),
    child: const SykiApp(),
  ));
}

class SykiApp extends StatelessWidget {
  const SykiApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: const HomePage(),
      debugShowCheckedModeBanner: false,
      theme: Provider.of<ThemeProvider>(context).themeData,
    );
  }
}
