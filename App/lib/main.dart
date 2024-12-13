import 'package:app/nav_menu.dart';
import 'package:flutter/material.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Syki',
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(
            seedColor: const Color.fromARGB(0, 119, 107, 231)),
        useMaterial3: true,
      ),
      home: const NavMenu(),
      debugShowCheckedModeBanner: false,
    );
  }
}
