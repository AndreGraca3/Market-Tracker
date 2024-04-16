plugins {
    id("com.android.application")
    id("org.jetbrains.kotlin.android")
    kotlin("kapt")
    id("com.google.dagger.hilt.android")
}

android {
    namespace = "com.example.markettracker"
    compileSdk = 34

    defaultConfig {
        applicationId = "com.example.markettracker"
        minSdk = 24
        targetSdk = 34
        versionCode = 1
        versionName = "1.0"

        testInstrumentationRunner = "androidx.test.runner.AndroidJUnitRunner"
        vectorDrawables {
            useSupportLibrary = true
        }
    }

    buildTypes {
        release {
            isMinifyEnabled = false
            proguardFiles(
                getDefaultProguardFile("proguard-android-optimize.txt"),
                "proguard-rules.pro"
            )
        }
    }
    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_1_8
        targetCompatibility = JavaVersion.VERSION_1_8
    }
    kotlinOptions {
        jvmTarget = "1.8"
    }
    buildFeatures {
        compose = true
    }
    composeOptions {
        kotlinCompilerExtensionVersion = "1.5.1"
    }
    packaging {
        resources {
            excludes += "/META-INF/{AL2.0,LGPL2.1}"
        }
    }
}

dependencies {

    implementation("androidx.core:core-ktx:1.12.0")
    implementation("androidx.lifecycle:lifecycle-runtime-ktx:2.7.0")
    implementation("androidx.activity:activity-compose:1.8.2")
    implementation(platform("androidx.compose:compose-bom:2024.02.01"))
    implementation("androidx.compose.ui:ui")
    implementation("androidx.compose.ui:ui-graphics")
    implementation("androidx.compose.ui:ui-tooling-preview")
    implementation("androidx.compose.material3:material3")
    implementation("androidx.navigation:navigation-compose:2.7.7")
    implementation("androidx.compose.material:material-icons-extended-android:1.6.4")
    implementation("androidx.core:core-splashscreen:1.0.1")
    implementation("io.coil-kt:coil-compose:2.6.0")

    implementation("com.google.dagger:hilt-android:2.49")
    kapt("com.google.dagger:hilt-android-compiler:2.49")
    implementation("androidx.hilt:hilt-navigation-compose:1.2.0")

    implementation(platform("com.squareup.okhttp3:okhttp-bom:4.11.0"))
    implementation("com.squareup.okhttp3:okhttp")
    implementation("com.google.code.gson:gson:2.10.1")
    implementation("androidx.datastore:datastore-preferences:1.0.0")
    implementation("androidx.datastore:datastore-core:1.0.0")

    implementation("com.exyte:animated-navigation-bar:1.0.0")
    implementation("com.github.tfaki:ComposableSweetToast:1.0.1")
    implementation("com.google.android.gms:play-services-auth:21.0.0")

    testImplementation("junit:junit:4.13.2")
    testImplementation("io.mockk:mockk-android:1.13.8")
    testImplementation("com.squareup.okhttp3:mockwebserver")

    androidTestImplementation("androidx.test.ext:junit:1.1.5")
    androidTestImplementation("androidx.test.espresso:espresso-core:3.5.1")
    androidTestImplementation(platform("androidx.compose:compose-bom:2023.08.00"))
    androidTestImplementation("androidx.compose.ui:ui-test-junit4")
    debugImplementation("androidx.compose.ui:ui-tooling")
    debugImplementation("androidx.compose.ui:ui-test-manifest")

    implementation("androidx.credentials:credentials:1.3.0-alpha02")

    // optional - needed for credentials support from play services, for devices running
    // Android 13 and below.
    implementation("androidx.credentials:credentials-play-services-auth:1.3.0-alpha02")

    implementation ("com.google.android.libraries.identity.googleid:googleid:1.1.0")
}

// Allow references to generated code
kapt {
    correctErrorTypes = true
}

hilt {
    enableAggregatingTask = true
}