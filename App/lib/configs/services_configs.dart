import 'package:get/get.dart';
import 'package:app/auth/auth_service.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

void addServicesConfigs() {
  Get.put(const FlutterSecureStorage());
  Get.put(AuthService());
}
