import 'package:app/configs/services_configs.dart';
import 'package:app/features/cross/create_pending_user_register/create_pending_user_register_in.dart';

class CreatePendingUserRegisterClient {
  Future<bool> create(String email) async {
    var data = CreatePendingUserRegisterIn(email: email);

    try {
      var response = await dio.post('/users', data: data.toJson());

      if (response.statusCode == 200) {
        return true;
      } else {
        return false;
      }
    } catch (e) {
      return false;
    }
  }
}
