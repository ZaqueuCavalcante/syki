import 'package:flutter/material.dart';

class NavMenu extends StatelessWidget {
  const NavMenu({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      bottomNavigationBar: NavigationBar(destinations: const [
        NavigationDestination(icon: Icon(Icons.home), label: 'Home'),
        NavigationDestination(icon: Icon(Icons.people), label: 'Time'),
        NavigationDestination(icon: Icon(Icons.book), label: 'Time'),
        NavigationDestination(
            icon: Icon(Icons.star_outline_outlined), label: 'Time'),
      ]),
    );
  }
}
