import 'dart:convert';
import 'package:app/configs/http_configs.dart';
import 'package:app/configs/services_configs.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class AuthService {
  final FlutterSecureStorage storage = getIt<FlutterSecureStorage>();

  Future<bool> login(String email, String password) async {
    try {
      final response = await dio.post(
        '/login',
        data: jsonEncode({'email': email, 'password': password}),
      );

      if (response.statusCode == 200) {
        final token = response.data;
        if (token != null) {
          await storage.write(key: 'jwt', value: token['accessToken']);
          return true;
        }
      }
    } catch (e) {
      print('Login error: $e');
    }
    return false;
  }

  Future<String?> getToken() async {
    return await storage.read(key: 'jwt');
  }
}
