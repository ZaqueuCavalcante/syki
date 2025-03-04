import 'package:get/get.dart';
import 'package:flutter/material.dart';
import 'package:app/auth/auth_service.dart';
import 'package:app/components/syki_text_field.dart';
import 'package:app/components/syki_primary_button.dart';

class LoginPage extends StatefulWidget {
  final void Function() goToRegisterPage;

  const LoginPage({super.key, required this.goToRegisterPage});

  @override
  State<LoginPage> createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  final TextEditingController emailController = TextEditingController();
  final TextEditingController passwordController = TextEditingController();

  final AuthService client = Get.find();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Theme.of(context).colorScheme.surface,
      body: SafeArea(
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 25),
          child: Center(
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                const SizedBox(height: 30),
                Icon(
                  Icons.lock_open_rounded,
                  size: 72,
                  color: Theme.of(context).colorScheme.primary,
                ),
                const SizedBox(height: 50),
                SykiTextField(
                  controller: emailController,
                  hintText: "Email",
                ),
                const SizedBox(height: 10),
                SykiTextField(
                  controller: passwordController,
                  hintText: "Senha",
                  obscureText: true,
                ),
                const SizedBox(height: 15),
                Padding(
                  padding: const EdgeInsets.only(right: 5),
                  child: Align(
                    alignment: Alignment.centerRight,
                    child: Text(
                      "Esqueci minha senha",
                      style: TextStyle(
                        color: Theme.of(context).colorScheme.primary,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                  ),
                ),
                const SizedBox(height: 30),
                SykiPrimaryButton(
                  text: "Login",
                  onTap: () async {
                    var result = await client.login(
                        emailController.text, passwordController.text);
                    var message = result ? "BOA" : "ERRO";
                    var snackBar = SnackBar(content: Text(message));

                    if (context.mounted) {
                      ScaffoldMessenger.of(context).showSnackBar(snackBar);
                    }
                  },
                ),
                const SizedBox(height: 40),
                Row(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Text(
                      "Primeiro acesso?",
                      style: TextStyle(
                        color: Theme.of(context).colorScheme.primary,
                      ),
                    ),
                    const SizedBox(width: 5),
                    GestureDetector(
                      onTap: widget.goToRegisterPage,
                      child: Text("Registre-se agora!",
                          style: TextStyle(
                              color: Theme.of(context).colorScheme.primary,
                              fontWeight: FontWeight.bold)),
                    )
                  ],
                )
              ],
            ),
          ),
        ),
      ),
    );
  }
}
