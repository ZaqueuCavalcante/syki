import 'package:go_router/go_router.dart';
import 'package:app/pages/settings_page.dart';
import 'package:app/pages/bootstrap_page.dart';

final router = GoRouter(
  initialLocation: '/',
  routes: [
    GoRoute(
      path: '/',
      builder: (context, state) => const BootstrapPage(),
    ),
    GoRoute(
      path: '/settings',
      builder: (context, state) => const SettingsPage(),
    ),
  ],
);
