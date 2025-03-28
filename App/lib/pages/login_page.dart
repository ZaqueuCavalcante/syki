import 'package:get/get.dart';
import 'package:flutter/material.dart';
import 'package:app/pages/home_page.dart';
import 'package:app/auth/auth_service.dart';
import 'package:app/pages/register_page.dart';
import 'package:app/constants/syki_sizes.dart';
import 'package:app/utils/syki_functions.dart';
import 'package:app/constants/syki_colors.dart';
import 'package:app/constants/syki_images.dart';

class LoginPage extends StatefulWidget {
  final void Function() goToRegisterPage;

  const LoginPage({super.key, required this.goToRegisterPage});

  @override
  State<LoginPage> createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  final TextEditingController emailController = TextEditingController();
  final TextEditingController passwordController = TextEditingController();

  final AuthService authService = Get.find();

  bool _showPassword = false;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SingleChildScrollView(
        child: Padding(
          padding: EdgeInsets.only(
            top: SykiSizes.appBarHeight,
            left: SykiSizes.defaultSpace,
            bottom: SykiSizes.defaultSpace,
            right: SykiSizes.defaultSpace,
          ),
          child: Column(
            children: [
              /// Logo + Title + SubTitle
              Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Image(
                    height: 130,
                    image: AssetImage(SykiImages.sykiLogo),
                  ),
                  Text(
                    'Bem-vindo(a)!',
                    style: Theme.of(context).textTheme.headlineMedium,
                  ),
                  const SizedBox(height: SykiSizes.sm),
                  Text(
                    'Simplifique sua vida acadêmica. Tudo sobre seu curso em um só lugar!',
                    style: Theme.of(context).textTheme.bodyMedium,
                  )
                ],
              ),

              const SizedBox(height: SykiSizes.spaceBtwSections),

              /// Form
              Form(
                child: Column(
                  children: [
                    TextFormField(
                      controller: emailController,
                      keyboardType: TextInputType.emailAddress,
                      textInputAction: TextInputAction.next,
                      onTapOutside: (PointerDownEvent event) {
                        FocusManager.instance.primaryFocus?.unfocus();
                      },
                      decoration: const InputDecoration(
                          prefixIcon: Icon(Icons.email_rounded),
                          labelText: 'Email'),
                    ),
                    const SizedBox(height: SykiSizes.spaceBtwInputFields),
                    TextFormField(
                      controller: passwordController,
                      keyboardType: TextInputType.text,
                      textInputAction: TextInputAction.done,
                      autofillHints: const [AutofillHints.password],
                      obscureText: !_showPassword,
                      onTapOutside: (PointerDownEvent event) {
                        FocusManager.instance.primaryFocus?.unfocus();
                      },
                      decoration: InputDecoration(
                          prefixIcon: Icon(Icons.lock_outline_rounded),
                          labelText: 'Senha',
                          suffixIcon: IconButton(
                              onPressed: () {
                                setState(() {
                                  _showPassword = !_showPassword;
                                });
                              },
                              icon: _showPassword
                                  ? Icon(Icons.visibility_off)
                                  : Icon(Icons.visibility))),
                    ),
                    Padding(
                      padding: const EdgeInsets.only(right: 5),
                      child: Align(
                        alignment: Alignment.centerRight,
                        child: TextButton(
                          onPressed: () {},
                          child: Text(
                            "Esqueci minha senha",
                            style: TextStyle(
                              fontWeight: FontWeight.w500,
                            ),
                          ),
                        ),
                      ),
                    ),
                  ],
                ),
              ),

              const SizedBox(height: SykiSizes.spaceBtwItems),

              SizedBox(
                width: double.infinity,
                child: ElevatedButton(
                  onPressed: () async {
                    var result = await authService.login(
                      emailController.text,
                      passwordController.text,
                    );
                    if (result) {
                      Get.to(() => HomePage());
                    }
                  },
                  child: Text('Login'),
                ),
              ),

              const SizedBox(height: SykiSizes.defaultSpace),
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Flexible(
                      child: Divider(
                    color: SykiFunctions.isDarkMode(context)
                        ? SykiColors.darkGrey
                        : SykiColors.grey,
                    thickness: 0.5,
                  ))
                ],
              ),
              const SizedBox(height: SykiSizes.defaultSpace),

              Text(
                'Ainda não definiu sua senha?',
                style: Theme.of(context).textTheme.bodyMedium,
              ),
              const SizedBox(height: SykiSizes.spaceBtwItems / 2),

              SizedBox(
                width: double.infinity,
                child: OutlinedButton(
                  onPressed: () {
                    Get.to(() => const RegisterPage());
                  },
                  child: Text('Primeiro acesso'),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
