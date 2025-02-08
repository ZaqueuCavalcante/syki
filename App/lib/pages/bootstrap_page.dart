import 'package:flutter/material.dart';
import 'package:app/pages/login_page.dart';
import 'package:app/pages/register_page.dart';

class BootstrapPage extends StatefulWidget {
  const BootstrapPage({super.key});

  @override
  State<BootstrapPage> createState() => _BootstrapPageState();
}

class _BootstrapPageState extends State<BootstrapPage> {
  bool showLoginPage = true;

  void toogleLoginRegisterPages() {
    setState(() {
      showLoginPage = !showLoginPage;
    });
  }

  @override
  Widget build(BuildContext context) {
    if (showLoginPage) {
      return LoginPage(
        goToRegisterPage: toogleLoginRegisterPages,
      );
    }

    return RegisterPage(
      goToLoginPage: toogleLoginRegisterPages,
    );
  }
}
