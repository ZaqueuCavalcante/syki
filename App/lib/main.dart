import 'package:app/configs/env.dart';
import 'package:app/configs/routing_configs.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:app/configs/http_configs.dart';
import 'package:app/themes/theme_provider.dart';
import 'package:app/configs/services_configs.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';

Future<void> main() async {
  await dotenv.load(fileName: Env.fileName);

  addServicesConfigs();
  addHttpConfigs();

  runApp(ChangeNotifierProvider(
    // AuthProvider
    //https://medium.com/@areesh-ali/building-a-secure-flutter-app-with-jwt-and-apis-e22ade2b2d5f
    create: (context) => ThemeProvider(),
    child: const SykiApp(),
  ));
}

class SykiApp extends StatelessWidget {
  const SykiApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp.router(
      routerConfig: router,
      debugShowCheckedModeBanner: false,
      theme: Provider.of<ThemeProvider>(context).themeData,
    );
  }
}
