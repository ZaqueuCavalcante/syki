import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:app/constants/syki_sizes.dart';
import 'package:app/constants/syki_images.dart';

class RegisterPage extends StatefulWidget {
  const RegisterPage({super.key});

  @override
  State<RegisterPage> createState() => _RegisterPageState();
}

class _RegisterPageState extends State<RegisterPage> {
  final TextEditingController codeController = TextEditingController();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(),
      body: SingleChildScrollView(
        child: Padding(
          padding: EdgeInsets.only(
            left: SykiSizes.defaultSpace,
            bottom: SykiSizes.defaultSpace,
            right: SykiSizes.defaultSpace,
          ),
          child: Column(
            children: [
              Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Image(
                    height: 130,
                    image: AssetImage(SykiImages.sykiLogo),
                  ),
                  const SizedBox(height: SykiSizes.sm),
                  Text(
                    'Informe o código de 6 dígitos que foi enviado para o seu email acadêmico:',
                    style: Theme.of(context).textTheme.bodyMedium,
                  )
                ],
              ),
              const SizedBox(height: SykiSizes.spaceBtwSections),
              Form(
                child: Column(
                  children: [
                    TextFormField(
                      maxLength: 6,
                      maxLengthEnforcement: MaxLengthEnforcement.enforced,
                      keyboardType: TextInputType.number,
                      onTapOutside: (PointerDownEvent event) {
                        FocusManager.instance.primaryFocus?.unfocus();
                      },
                      inputFormatters: [FilteringTextInputFormatter.digitsOnly],
                      decoration: const InputDecoration(
                          prefixIcon: Icon(Icons.token_rounded),
                          labelText: 'Código'),
                    ),
                  ],
                ),
              ),
              const SizedBox(height: SykiSizes.spaceBtwItems),
              SizedBox(
                width: double.infinity,
                child: ElevatedButton(
                  onPressed: () {},
                  child: Text('Avançar'),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
