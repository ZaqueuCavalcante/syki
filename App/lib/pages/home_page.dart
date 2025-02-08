import 'package:flutter/material.dart';
import 'package:app/components/syki_drawer.dart';

class HomePage extends StatefulWidget {
  const HomePage({super.key});

  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Theme.of(context).colorScheme.surface,
      drawer: const SykiDrawer(),
      appBar: AppBar(title: const Text("Syki")),
    );
  }
}
