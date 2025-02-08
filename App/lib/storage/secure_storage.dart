import 'package:flutter_secure_storage/flutter_secure_storage.dart';

const secureStorage = FlutterSecureStorage();

Future<void> saveToken(String token) async {
  await secureStorage.write(key: 'jwt', value: token);
}

Future<String?> getToken() async {
  return await secureStorage.read(key: 'jwt');
}

Future<void> deleteToken() async {
  await secureStorage.delete(key: 'jwt');
}
