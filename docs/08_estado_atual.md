# Estado Atual

Ultima atualizacao: 2026-07-07.

## Projeto

- Nome: Terra Sangrada.
- Genero: hack and slash / beat 'em up 2D.
- Engine: Godot .NET 4.7.
- Linguagem: C#.
- Runtime local: .NET 10.0.
- Target framework do jogo: `net10.0`.
- Tom: adulto, dark, sangrento, melancolico.

## Sprint atual

Sprint 1 - Combate Prototipo.

Status: validada localmente no Godot.

Sprint 1 fechada com:

- Build .NET 10 aprovado.
- Godot console sem erro de assembly/script.
- Validacao visual aprovada pelo usuario.
- Cena `scenes/Main.tscn` rodando localmente.

## Implementado

- Projeto Godot criado.
- Projeto C# criado.
- Cena principal `scenes/Main.tscn`.
- Arena prototipo `scenes/levels/PrototypeArena.tscn` agora visivel tambem no editor do Godot, nao apenas em runtime.
- Player placeholder com movimento `WASD`.
- Ataque leve em `J`.
- Estados basicos de player: idle, walk e light attack.
- Hitbox de ataque separada da colisao do player.
- Inimigos placeholder com hurtbox, vida, hit flash, hit stun, knockback e morte.
- Inimigos se aproximam lentamente do player.
- Camera seguindo o player.
- Primeiro passe de hit pause e screen shake.
- Limites de movimento no eixo Y.
- Documentos de producao e agentes.

## Validacao feita

- `dotnet restore TerraSangrada.csproj`: sucesso.
- `dotnet build TerraSangrada.csproj --no-restore`: sucesso, 0 erros e 0 avisos, executado localmente no Desktop.
- Godot encontrado e aberto em 2026-07-07 pelo caminho local informado pelo usuario.
- Godot console validado sem erros de assembly/script apos corrigir `[dotnet] project/assembly_name="TerraSangrada"`.
- Validacao visual aprovada pelo usuario em 2026-07-07.

Godot local:

```text
C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64.exe
```

Repositorio local principal para desenvolvimento e validacao:

```text
C:\Users\mober\OneDrive\Desktop\fauna-do-sangue
```

## Proximo passo

Criar commit local da Sprint 1 validada e subir para o GitHub. Depois iniciar Sprint 2: vertical slice visual.

## Riscos atuais

- Godot nao foi encontrado no PATH do ambiente atual.
- Godot abre por caminho absoluto local.
- Inimigos ainda nao atacam; isso fica para a sprint de sistema de inimigos.
