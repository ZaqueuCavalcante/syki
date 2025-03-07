import 'package:get/get.dart';
import 'package:flutter/material.dart';
import 'package:app/global/nav_menu.dart';
import 'package:app/auth/auth_service.dart';

class HomePage extends StatefulWidget {
  const HomePage({super.key});

  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  final AuthService authService = Get.find();
  String? _payload;

  @override
  void initState() {
    super.initState();
    init();
  }

  init() async {
    var value = await authService.getToken();
    setState(() {
      _payload = value;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: NavMenu(),
    );
  }
}
