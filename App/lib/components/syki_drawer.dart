import 'package:app/components/syki_drawer_tile.dart';
import 'package:app/pages/settings_page.dart';
import 'package:flutter/material.dart';

class SykiDrawer extends StatelessWidget {
  const SykiDrawer({super.key});

  @override
  Widget build(BuildContext context) {
    return Drawer(
      backgroundColor: Theme.of(context).colorScheme.surface,
      child: SafeArea(
          child: Column(
        children: [
          Padding(
            padding: const EdgeInsets.symmetric(vertical: 50),
            child: Icon(
              Icons.person,
              size: 72,
              color: Theme.of(context).colorScheme.primary,
            ),
          ),
          Divider(
            indent: 25,
            endIndent: 25,
            color: Theme.of(context).colorScheme.secondary,
          ),
          SykiDrawerTile(
            title: "Home",
            icon: Icons.home,
            onTap: () {
              Navigator.pop(context);
            },
          ),
          SykiDrawerTile(
            title: "Settings",
            icon: Icons.settings,
            onTap: () {
              Navigator.pop(context);

              Navigator.push(context,
                  MaterialPageRoute(builder: (context) => SettingsPage()));
            },
          )
        ],
      )),
    );
  }
}
